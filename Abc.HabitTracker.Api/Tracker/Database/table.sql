CREATE TABLE badges
(
    id uuid NOT NULL,
    name character varying COLLATE pg_catalog."default",
    description character varying COLLATE pg_catalog."default",
    user_id uuid,
    created_at time with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT badges_pkey PRIMARY KEY (id),
    CONSTRAINT badges_user_id_fkey FOREIGN KEY (user_id)
        REFERENCES public.users (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
        NOT VALID
)

CREATE TABLE public.habits
(
    id uuid NOT NULL,
    name character varying COLLATE pg_catalog."default",
    day_off character varying[] COLLATE pg_catalog."default",
    current_streak integer NOT NULL DEFAULT 0,
    longest_streak integer NOT NULL DEFAULT 0,
    log_count integer NOT NULL DEFAULT 0,
    logs timestamp with time zone[] NOT NULL DEFAULT ARRAY[]::timestamp with time zone[],
    user_id uuid,
    created_at timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT habits_pkey PRIMARY KEY (id),
    CONSTRAINT user_fkey FOREIGN KEY (user_id)
        REFERENCES public.users (id) MATCH SIMPLE
        ON UPDATE CASCADE
        ON DELETE CASCADE
)

CREATE TABLE public.streak_snapshot
(
    id integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
    current_streak integer,
    user_id uuid,
    habit_id uuid,
    CONSTRAINT streak_snapshot_pkey PRIMARY KEY (id),
    CONSTRAINT streak_snapshot_habit_id_fkey FOREIGN KEY (habit_id)
        REFERENCES public.habits (id) MATCH SIMPLE
        ON UPDATE SET DEFAULT
        ON DELETE SET DEFAULT
        NOT VALID,
    CONSTRAINT streak_snapshot_user_id_fkey FOREIGN KEY (user_id)
        REFERENCES public.users (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
        NOT VALID
)

CREATE TABLE users
(
    id uuid NOT NULL,
    name character varying COLLATE pg_catalog."default",
    CONSTRAINT users_pkey PRIMARY KEY (id)
)

CREATE TABLE workaholic_snapshot
(
    id integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
    date timestamp with time zone,
    user_id uuid NOT NULL,
    CONSTRAINT workaholic_snapshot_pkey PRIMARY KEY (id),
    CONSTRAINT workaholic_snapshot_user_id_fkey FOREIGN KEY (user_id)
        REFERENCES public.users (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)