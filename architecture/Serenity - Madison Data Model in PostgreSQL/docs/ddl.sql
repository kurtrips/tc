--
-- PostgreSQL database dump
--

SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;

--
-- Name: postgres; Type: COMMENT; Schema: -; Owner: postgres
--

COMMENT ON DATABASE postgres IS 'default administrative connection database';


--
-- Name: plpgsql; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS plpgsql WITH SCHEMA pg_catalog;


--
-- Name: EXTENSION plpgsql; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION plpgsql IS 'PL/pgSQL procedural language';


SET search_path = public, pg_catalog;

--
-- Name: requirement_neccesity; Type: TYPE; Schema: public; Owner: postgres
--

CREATE TYPE requirement_neccesity AS ENUM (
    'Must',
    'Should',
    'Nice',
    'Optional'
);


ALTER TYPE public.requirement_neccesity OWNER TO postgres;

--
-- Name: requirement_type; Type: TYPE; Schema: public; Owner: postgres
--

CREATE TYPE requirement_type AS ENUM (
    'Functional',
    'Technical',
    'Informational',
    'Other'
);


ALTER TYPE public.requirement_type OWNER TO postgres;

SET default_tablespace = '';

SET default_with_oids = false;

--
-- Name: challenge_requirements; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE challenge_requirements (
    challenge_id bigint NOT NULL,
    requirement_id bigint NOT NULL
);


ALTER TABLE public.challenge_requirements OWNER TO postgres;

--
-- Name: requirement; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE requirement (
    requirement_id bigserial NOT NULL,
    type requirement_type NOT NULL,
    body text NOT NULL,
    score_min smallint NOT NULL,
    score_max smallint NOT NULL,
    tags text,
    created_by_user_id bigint NOT NULL,
    is_private bit(1) DEFAULT (1)::bit(1) NOT NULL,
    difficulty smallint,
    necessity requirement_neccesity,
    textsearchable_body tsvector,
    is_in_library bit(1) DEFAULT (0)::bit(1) NOT NULL,
    weight numeric(5,3) DEFAULT 1 NOT NULL
);


ALTER TABLE public.requirement OWNER TO postgres;

--
-- Name: scorecard; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE scorecard (
    scorecard_id bigserial NOT NULL,
    submitter_id bigint NOT NULL,
    reviewer_id bigint NOT NULL,
    challenge_id bigint NOT NULL,
    initial_score decimal(8,3) NOT NULL DEFAULT 0,
    final_score decimal(8,3) NOT NULL DEFAULT 0
);


ALTER TABLE public.scorecard OWNER TO postgres;

--
-- Name: scorecard_item; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE scorecard_item (
    scorecard_id bigint NOT NULL,
    requirement_id bigint NOT NULL,
    initial_score smallint,
    final_score smallint,
    appeal text,
    appeal_response text
);


ALTER TABLE public.scorecard_item OWNER TO postgres;

--
-- Name: tag; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE tag (
    tag_name character varying(64) NOT NULL
);


ALTER TABLE public.tag OWNER TO postgres;

--
-- Name: challenge_requirements_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY challenge_requirements
    ADD CONSTRAINT challenge_requirements_pkey PRIMARY KEY (challenge_id, requirement_id);


--
-- Name: requirement_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY requirement
    ADD CONSTRAINT requirement_pkey PRIMARY KEY (requirement_id);


--
-- Name: scorecard_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY scorecard
    ADD CONSTRAINT scorecard_pkey PRIMARY KEY (scorecard_id);


--
-- Name: tag_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY tag
    ADD CONSTRAINT tag_pkey PRIMARY KEY (tag_name);


--
-- Name: fk_challenge_requirements_1_idx; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX fk_challenge_requirements_1_idx ON challenge_requirements USING btree (requirement_id);


--
-- Name: fk_scorecard_item_1_idx; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX fk_scorecard_item_1_idx ON scorecard_item USING btree (requirement_id);


--
-- Name: fk_scorecard_item_2_idx; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX fk_scorecard_item_2_idx ON scorecard_item USING btree (scorecard_id);


--
-- Name: idx_scorecard_challenge_id; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX idx_scorecard_challenge_id ON scorecard USING btree (challenge_id);


--
-- Name: idx_scorecard_reviewer_id; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX idx_scorecard_reviewer_id ON scorecard USING btree (reviewer_id);


--
-- Name: idx_scorecard_submitter_id; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX idx_scorecard_submitter_id ON scorecard USING btree (submitter_id);


--
-- Name: pgreq_idx1; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX pgreq_idx1 ON requirement USING gin (textsearchable_body);

--
-- Name: pgreq_idx2; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX pgreq_idx2 ON requirement USING gin (tags);

--
-- Name: fk_challenge_requirements_1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY challenge_requirements
    ADD CONSTRAINT fk_challenge_requirements_1 FOREIGN KEY (requirement_id) REFERENCES requirement(requirement_id);


--
-- Name: fk_scorecard_item_1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY scorecard_item
    ADD CONSTRAINT fk_scorecard_item_1 FOREIGN KEY (requirement_id) REFERENCES requirement(requirement_id);


--
-- Name: fk_scorecard_item_2; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY scorecard_item
    ADD CONSTRAINT fk_scorecard_item_2 FOREIGN KEY (scorecard_id) REFERENCES scorecard(scorecard_id);


CREATE TRIGGER trg_req_body BEFORE INSERT OR UPDATE of body ON requirement 
FOR EACH ROW EXECUTE PROCEDURE create_tsv_body();

CREATE FUNCTION create_tsv_body() RETURNS TRIGGER AS $_$
BEGIN
    NEW.textsearchable_body = to_tsvector('english', NEW.body);
    RETURN NEW;
END $_$ LANGUAGE 'plpgsql';

--
-- Name: public; Type: ACL; Schema: -; Owner: postgres
--

REVOKE ALL ON SCHEMA public FROM PUBLIC;
REVOKE ALL ON SCHEMA public FROM postgres;
GRANT ALL ON SCHEMA public TO postgres;
GRANT ALL ON SCHEMA public TO PUBLIC;


--
-- PostgreSQL database dump complete
--

